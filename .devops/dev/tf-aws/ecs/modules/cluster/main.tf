################################################################################
# Cluster
################################################################################

locals {

}

resource "aws_ecs_cluster" "this" {
  name = var.cluster_name

  dynamic "service_connect_defaults" {
    for_each = length(var.cluster_service_connect_defaults) > 0 ? [var.cluster_service_connect_defaults] : []
    
    content {
        namespace = service_connect_defaults.value.namespace
    }
  }

  dynamic "setting" {
    for_each = flattern([var.cluster_settings])
    
    content {
        name  = setting.value.name
        value = setting.value.value
    }
  }

  tags = var.tags
}

################################################################################
# Cluster Capacity Providers
################################################################################

locals {
    default_capacity_providers = merge(
      { for k, v in var.fargate_capacity_providers : k => v if var.default_capacity_provider_use_fargate },
      { for k, v in var.autoscaling_capacity_providers : k => v if !var.default_capacity_provider_use_fargate }
    )
}

resource "aws_ecs_cluster_capacity_providers" "this" {
  count = var.create && length(merge(var.fargate_capacity_providers, var.autoscaling_capacity_providers)) > 0 ? 1 : 0

  cluster_name = aws_ecs_cluster.this[0].name
  capacity_providers = distinct(concat(
    [for k, v in var.fargate_capacity_providers : try(v.name, k)],
    [for k, v in var.autoscaling_capacity_providers : try(v.name, k)]
  ))

  # https://docs.aws.amazon.com/AmazonECS/latest/developerguide/cluster-capacity-providers.html#capacity-providers-considerations
  dynamic "default_capacity_provider_strategy" {
    for_each = local.default_capacity_providers
    iterator = strategy

    content {
      capacity_provider = try(strategy.value.name, strategy.key)
      base              = try(strategy.value.default_capacity_provider_strategy.base, null)
      weight            = try(strategy.value.default_capacity_provider_strategy.weight, null)
    }
  }

  depends_on = [
    aws_ecs_capacity_provider.this
  ]
}
