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

# Create HTTPS certificate
resource "google_compute_managed_ssl_certificate" "website" {
  provider = google
  name     = "website-cert"
  managed {
    domains = [google_dns_record_set.website.name]
  }
}

resource "google_compute_url_map" "website" {
  provider        = google
  name            = "website-url-map"
  default_service = google_compute_backend_bucket.website.self_link

  path_matcher {
    name            = "api"
    default_service = google_compute_backend_bucket.website.self_link

    path_rule {
      paths   = ["/api", "/api/*"]
      service = google_compute_backend_service.gke_primary_cluster_backend.self_link
    }
  }
}

resource "google_compute_target_https_proxy" "website" {
  provider         = google
  name             = "website-target-proxy"
  url_map          = google_compute_url_map.website.self_link
  ssl_certificates = [google_compute_managed_ssl_certificate.website.self_link]
}

# Frontend forwarding
resource "google_compute_global_forwarding_rule" "default" {
  provider              = google
  name                  = "website-forwarding-rule"
  load_balancing_scheme = "EXTERNAL"
  ip_address            = google_compute_global_address.website.address
  ip_protocol           = "TCP"
  port_range            = "443"
  target                = google_compute_target_https_proxy.website.self_link
}
