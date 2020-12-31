variable "project_id" {
  type        = string
  description = "The Google Cloud Project Id"
}

variable "region" {
  type    = string
  default = "europe-west2"
}

variable "admin_member" {
  type        = string
  description = "Admin member alias (https://registry.terraform.io/providers/hashicorp/google/latest/docs/resources/storage_bucket_iam#member/members)"
}