# Bucket 
resource "google_storage_bucket" "frontend" {
  provider                    = google
  name                        = "wwww.elevait.co.uk"
  location                    = "EUROPE-WEST2"
  uniform_bucket_level_access = true

  website {
    main_page_suffix = "index.html"
    not_found_page   = "index.html"
  }
}

# Bucket Deployment iam user
resource "google_service_account" "frontend_bucket_deployment_sa" {
  account_id   = "frontend-bucket-deployment-sa"
  display_name = "frontend_bucket_deployment Account"
}

resource "google_storage_bucket_iam_member" "frontend_bucket_deployment_sa-iam" {
  bucket = google_storage_bucket.frontend.name
  role   = "roles/storage.admin"
  member = "serviceAccount:${google_service_account.frontend_bucket_deployment_sa.email}"
}

resource "google_storage_bucket_iam_member" "frontend_bucket_terraform_allusers_view" {
  bucket = google_storage_bucket.frontend.name
  role   = "roles/storage.objectViewer"
  member = "allUsers"
}

resource "google_storage_bucket_iam_member" "frontend_bucket_terraform_sa-iam" {
  bucket = google_storage_bucket.frontend.name
  role   = "roles/storage.admin"
  member = "serviceAccount:terraform@${var.project_id}.iam.gserviceaccount.com"
}

# Reserve an external IP
resource "google_compute_global_address" "website" {
  provider = google
  name     = "website-lb-ip"
}

# Get the managed DNS zone
resource "google_dns_managed_zone" "gcp_cycling_buddies" {
  provider = google
  name     = "gcp-cycling-buddies"
  dns_name = "elevait.co.uk."
}

# Add the IP to the DNS
resource "google_dns_record_set" "website" {
  provider     = google
  name         = google_dns_managed_zone.gcp_cycling_buddies.dns_name
  type         = "A"
  ttl          = 300
  managed_zone = google_dns_managed_zone.gcp_cycling_buddies.name
  rrdatas      = [google_compute_global_address.website.address]
}

# Add the bucket as a CDN backend
resource "google_compute_backend_bucket" "website" {
  provider    = google
  name        = "website-backend"
  description = "Contains files needed by the frontend"
  bucket_name = google_storage_bucket.frontend.name
  enable_cdn  = true
}

# Create HTTPS certificate
resource "google_compute_managed_ssl_certificate" "website" {
  provider = google
  name     = "website-cert"
  managed {
    domains = [google_dns_record_set.website.name]
  }
}

# GCP URL MAP
resource "google_compute_url_map" "website" {
  provider        = google
  name            = "website-url-map"
  default_service = google_compute_backend_bucket.website.self_link
}

# GCP target proxy
resource "google_compute_target_https_proxy" "website" {
  provider         = google
  name             = "website-target-proxy"
  url_map          = google_compute_url_map.website.self_link
  ssl_certificates = [google_compute_managed_ssl_certificate.website.self_link]
}

# GCP forwarding rule
resource "google_compute_global_forwarding_rule" "default" {
  provider              = google
  name                  = "website-forwarding-rule"
  load_balancing_scheme = "EXTERNAL"
  ip_address            = google_compute_global_address.website.address
  ip_protocol           = "TCP"
  port_range            = "443"
  target                = google_compute_target_https_proxy.website.self_link
}