resource "google_storage_bucket" "frontend" {
  provider = google
  name     = "${var.project_id}-frontend"
  location = "EUROPE-WEST2"
}

resource "google_storage_default_object_access_control" "website_read" {
  bucket = google_storage_bucket.frontend.name
  role   = "READER"
  entity = "allUsers"
}