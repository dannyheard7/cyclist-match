# Container registries
resource "google_container_registry" "image_registry" {
  project  = var.project_id
  location = "EU"
}

resource "google_storage_bucket_iam_member" "image_registry_admin_member" {
  bucket = google_container_registry.image_registry.id
  role   = "roles/storage.admin"
  member = var.admin_member
}

resource "google_storage_bucket_iam_member" "image_registry_terraform_sa_iam" {
  bucket = google_container_registry.image_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:${local.terraform_sa_email}"
}


# Image Deployment iam user
resource "google_service_account" "gcr_image_deployment_sa" {
  account_id   = "gcr-image-deployment-sa"
  display_name = "gcr_image_deployment Account"
}

resource "google_storage_bucket_iam_member" "image_registry_gcr_image_deployment_sa-iam" {
  bucket = google_container_registry.image_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:${google_service_account.gcr_image_deployment_sa.email}"
}
