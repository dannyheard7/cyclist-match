provider "google" {
  project = var.project_id
  region  = var.region
}


terraform {
  backend "gcs" {
    bucket = "organic-area-299215-tfstate"
    prefix = "root\tfsate"
  }
}

provider "google-beta" {
  project = var.project_id
  region  = var.region
}

