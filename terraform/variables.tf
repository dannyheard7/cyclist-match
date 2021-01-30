variable "project_id" {
  type        = string
  description = "The Google Cloud Project Id"
}

variable "region" {
  type    = string
  default = "us-central1"
}

variable "admin_member" {
  type        = string
  description = "Admin member alias (https://registry.terraform.io/providers/hashicorp/google/latest/docs/resources/storage_bucket_iam#member/members)"
}

variable "frontend_bucket_name" {
  type    = string
  default = "www.elevait.co.uk"
}

variable "helm_version" {
  type        = string
  description = "Helm Version"
  default     = "3.4.2"
}

variable "helm_sa_name" {
  type        = string
  description = "Helm Service Account Name"
}
