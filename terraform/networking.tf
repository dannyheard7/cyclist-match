data "google_compute_network" "default" {
  name = "default"
}

resource "google_compute_router" "default_router" {
  name    = "default-router"
  network = data.google_compute_network.default.name
  region  = var.region
}

resource "google_compute_router_nat" "nat" {
  name                               = "default-router-nat"
  router                             = google_compute_router.default_router.name
  region                             = google_compute_router.default_router.region
  nat_ip_allocate_option             = "AUTO_ONLY"
  source_subnetwork_ip_ranges_to_nat = "ALL_SUBNETWORKS_ALL_IP_RANGES"

  log_config {
    enable = true
    filter = "ERRORS_ONLY"
  }
}


# data "google_compute_network_endpoint_group" "gke_negs" {
#   for_each = toset(["Todd", "James", "Alice", "Dottie"]) //  This will be the names of the generated negs
#   name     = each.key
# }
