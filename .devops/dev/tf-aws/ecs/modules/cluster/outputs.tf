################################################################################
# Cluster
################################################################################

output "arn" {
  description = "ARN that identifies the cluster"
  value       = try(aws_ecs_cluster.this[0].arn, null)
}

output "id" {
  description = "ID that identifies the cluster"
  value       = try(aws_ecs_cluster.this[0].id, null)
}

output "name" {
  description = "Name that identifies the cluster"
  value       = try(aws_ecs_cluster.this[0].name, null)
}

################################################################################
# Cluster Capacity Providers
################################################################################

output "cluster_capacity_providers" {
  description = "Map of cluster capacity providers attributes"
  value       = { for k, v in aws_ecs_cluster_capacity_providers.this : v.id => v }
}
