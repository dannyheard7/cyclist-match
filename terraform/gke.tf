resource "google_container_cluster" "primary" {
  provider = google

  name     = "${var.project_id}-gke-1"
  project  = var.project_id
  location = "us-central1"

  remove_default_node_pool = true
  initial_node_count       = 1

  addons_config {
    http_load_balancing {
      disabled = true
    }

    horizontal_pod_autoscaling {
      disabled = true
    }
  }

  master_auth {
    username = ""
    password = ""

    client_certificate_config {
      issue_client_certificate = false
    }
  }
}

resource "google_container_node_pool" "primary_preemptible_nodes" {
  name           = "my-node-pool"
  location       = "us-central1"
  cluster        = google_container_cluster.primary.name
  node_count     = 1
  node_locations = ["us-central1-a"]

  node_config {
    preemptible  = true
    machine_type = "g1-small"

    metadata = {
      disable-legacy-endpoints = "true"
    }

    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform"
    ]
  }
}

provider "kubernetes" {
  load_config_file = false

  host  = "https://${google_container_cluster.primary.endpoint}"
  token = data.google_client_config.current.access_token
  cluster_ca_certificate = base64decode(
    google_container_cluster.primary.master_auth[0].cluster_ca_certificate,
  )
}

resource "kubernetes_service_account" "helm_account" {
  provider = kubernetes
  depends_on = [
    google_container_cluster.primary,
  ]
  metadata {
    name      = var.helm_sa_name
    namespace = "kube-system"
  }
}

resource "kubernetes_cluster_role_binding" "helm_role_binding" {
  metadata {
    name = kubernetes_service_account.helm_account.metadata.0.name
  }
  role_ref {
    api_group = "rbac.authorization.k8s.io"
    kind      = "ClusterRole"
    name      = "cluster-admin"
  }
  subject {
    api_group = ""
    kind      = "ServiceAccount"
    name      = kubernetes_service_account.helm_account.metadata.0.name
    namespace = "kube-system"
  }
  provisioner "local-exec" {
    command = "sleep 15"
  }
}

provider "helm" {
  service_account = kubernetes_service_account.helm_account.metadata.0.name
  tiller_image    = "gcr.io/kubernetes-helm/tiller:${var.helm_version}"

  kubernetes {
    host                   = google_container_cluster.primary.endpoint
    token                  = data.google_client_config.current.access_token
    client_certificate     = base64decode(google_container_cluster.default.master_auth.0.client_certificate)
    client_key             = base64decode(google_container_cluster.default.master_auth.0.client_key)
    cluster_ca_certificate = base64decode(google_container_cluster.default.master_auth.0.cluster_ca_certificate)
  }
}
