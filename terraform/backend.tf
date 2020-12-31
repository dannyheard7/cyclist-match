# Container registries
resource "google_container_registry" "api_registry" {
  project  = var.project_id
  location = "EU"
}

resource "google_storage_bucket_iam_member" "api_registry_terraform_sa_iam" {
  bucket = google_container_registry.api_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:terraform@${var.project_id}.iam.gserviceaccount.com"
}

resource "google_container_registry" "db_migration_registry" {
  project  = var.project_id
  location = "EU"
}

resource "google_storage_bucket_iam_member" "db_migration_registry_terraform_sa_iam" {
  bucket = google_container_registry.db_migration_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:terraform@${var.project_id}.iam.gserviceaccount.com"
}

# Image Deployment iam user
resource "google_service_account" "gcr_image_deployment_sa" {
  account_id   = "gcr-image-deployment-sa"
  display_name = "gcr_image_deployment Account"
}

resource "google_storage_bucket_iam_member" "db_migration_registry_gcr_image_deployment_sa-iam" {
  bucket = google_container_registry.db_migration_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:${google_service_account.gcr_image_deployment_sa.email}"
}

resource "google_storage_bucket_iam_member" "api_registry_gcr_image_deployment_sa-iam" {
  bucket = google_container_registry.api_registry.id
  role   = "roles/storage.admin"
  member = "serviceAccount:${google_service_account.gcr_image_deployment_sa.email}"
}