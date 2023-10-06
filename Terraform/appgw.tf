# An application gateway for the web service
resource "azurerm_application_gateway" "appgw" {
  name                = var.appgw_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location

  sku {
    name = var.appgw_tier[terraform.workspace]
    tier = var.appgw_tier[terraform.workspace]
  }

  autoscale_configuration {
    min_capacity = var.autoscale_min[terraform.workspace]
    max_capacity = var.autoscale_max[terraform.workspace]
  }

  # Dynamic block that only applies to Load-Test and Prod environments
  dynamic "waf_configuration" {
    for_each = [
      for rg in data.azurerm_resource_group.rg : rg
      if data.azurerm_resource_group.rg.name == "s185p01-childrens-social-care-cpd-rg" && rg == "s185p01-childrens-social-care-cpd-rg" || data.azurerm_resource_group.rg.name == "s185d03-childrens-social-care-cpd-rg" && rg == "s185d03-childrens-social-care-cpd-rg"
    ]

    content {
      enabled          = true
      firewall_mode    = "Prevention"
      rule_set_version = "3.2"
    }
  }

  # A firewall policy that should only be populated for Load-Test and Prod environments 
  firewall_policy_id = var.appgw_tier[terraform.workspace] == "WAF_v2" ? azurerm_web_application_firewall_policy.fwpol.id : null

  gateway_ip_configuration {
    name      = var.gateway_ip_configuration[terraform.workspace]
    subnet_id = azurerm_subnet.frontend.id
  }

  frontend_port {
    name = var.frontend_port_name[terraform.workspace]
    port = 80
  }

  frontend_port {
    name = var.frontend_ssl_port_name[terraform.workspace]
    port = 443
  }

  frontend_ip_configuration {
    name                 = var.frontend_ip_configuration_name[terraform.workspace]
    public_ip_address_id = data.azurerm_public_ip.pip1.id
  }

  backend_address_pool {
    name  = var.backend_address_pool_name[terraform.workspace]
    fqdns = [azurerm_linux_web_app.linux-web-app.default_hostname]
  }

  backend_http_settings {
    name                                = var.http_setting_name[terraform.workspace]
    pick_host_name_from_backend_address = true
    cookie_based_affinity               = "Disabled"
    path                                = "/"
    port                                = 80
    protocol                            = "Http"
    request_timeout                     = 30
    probe_name                          = var.appgw_probe[terraform.workspace]
  }

  http_listener {
    name                           = var.listener_name[terraform.workspace]
    frontend_ip_configuration_name = var.frontend_ip_configuration_name[terraform.workspace]
    frontend_port_name             = var.frontend_port_name[terraform.workspace]
    protocol                       = "Http"
  }

  http_listener {
    name                           = var.ssl_listener_name[terraform.workspace]
    frontend_ip_configuration_name = var.frontend_ip_configuration_name[terraform.workspace]
    frontend_port_name             = var.frontend_ssl_port_name[terraform.workspace]
    protocol                       = "Https"
    ssl_certificate_name           = "develop-child-family-social-work-career-23"
    host_names                     = data.azurerm_resource_group.rg.name == "s185p01-childrens-social-care-cpd-rg" ? ["www.develop-child-family-social-work-career.education.gov.uk", "develop-child-family-social-work-career.education.gov.uk", ] : null
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [data.azurerm_user_assigned_identity.uai.id]
  }

  ssl_certificate {
    name                = "develop-child-family-social-work-career-23"
    key_vault_secret_id = "${var.key_vault_url[terraform.workspace]}secrets/develop-child-family-social-work-career-23"
  }

  ssl_policy {
    policy_type = "Predefined"
    policy_name = "AppGwSslPolicy20170401S"
  }

  redirect_configuration {
    name                 = var.redirect_config_name[terraform.workspace]
    redirect_type        = "Permanent"
    include_path         = true
    include_query_string = true
    target_listener_name = var.ssl_listener_name[terraform.workspace]
  }

  request_routing_rule {
    name                        = var.request_routing_rule_name[terraform.workspace]
    rule_type                   = "Basic"
    redirect_configuration_name = var.redirect_config_name[terraform.workspace]
    priority                    = 2001
    http_listener_name          = var.listener_name[terraform.workspace]
  }

  request_routing_rule {
    name                       = var.request_ssl_routing_rule_name[terraform.workspace]
    rule_type                  = "Basic"
    priority                   = 2000
    http_listener_name         = var.ssl_listener_name[terraform.workspace]
    backend_address_pool_name  = var.backend_address_pool_name[terraform.workspace]
    backend_http_settings_name = var.http_setting_name[terraform.workspace]
    rewrite_rule_set_name      = var.appgw_rewrite_rule_set[terraform.workspace]
  }

  probe {
    name                                      = var.appgw_probe[terraform.workspace]
    pick_host_name_from_backend_http_settings = true
    path                                      = "/application/status"
    interval                                  = 30
    timeout                                   = 30
    unhealthy_threshold                       = 3
    protocol                                  = "Http"
  }

  custom_error_configuration {
    status_code           = "HttpStatus403"
    custom_error_page_url = "https://s185errorpage.blob.core.windows.net/s185errorpage/403.html"
  }

  custom_error_configuration {
    status_code           = "HttpStatus502"
    custom_error_page_url = "https://s185errorpage.blob.core.windows.net/s185errorpage/502.html"
  }

  rewrite_rule_set {
    name = var.appgw_rewrite_rule_set[terraform.workspace]

    rewrite_rule {
      name          = var.appgw_rewrite_rule[terraform.workspace]
      rule_sequence = 1

      response_header_configuration {
        header_name  = "X-Frame-Options"
        header_value = "SAMEORIGIN"
      }

      response_header_configuration {
        header_name  = "X-Xss-Protection"
        header_value = "0"
      }

      response_header_configuration {
        header_name  = "X-Content-Type-Options"
        header_value = "nosniff"
      }

      response_header_configuration {
        header_name  = "Content-Security-Policy"
        header_value = "upgrade-insecure-requests; base-uri 'self'; frame-ancestors 'self'; form-action 'self'; object-src 'none';"
      }

      response_header_configuration {
        header_name  = "Referrer-Policy"
        header_value = "strict-origin-when-cross-origin"
      }

      response_header_configuration {
        header_name  = "Strict-Transport-Security"
        header_value = "max-age=31536000; includeSubDomains; preload"
      }

      response_header_configuration {
        header_name  = "Permissions-Policy"
        header_value = "accelerometer=(), ambient-light-sensor=(), autoplay=(), camera=(), encrypted-media=(), fullscreen=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), midi=(), payment=(), picture-in-picture=(), speaker=(), sync-xhr=self, usb=(), vr=()"
      }

      response_header_configuration {
        header_name  = "Server"
        header_value = ""
      }

      response_header_configuration {
        header_name  = "X-Powered-By"
        header_value = ""
      }
    }
  }

  tags = data.azurerm_resource_group.rg.tags
}

# A firewall policy that is only attached for Load-Test and Prod environments
resource "azurerm_web_application_firewall_policy" "fwpol" {
  name                = var.fwpol_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name

  managed_rules {
    managed_rule_set {
      type    = "OWASP"
      version = "3.2"
    }

    managed_rule_set {
      type    = "Microsoft_BotManagerRuleSet"
      version = "0.1"
    }
  }

  policy_settings {
    mode = "Prevention"
  }

  tags = data.azurerm_resource_group.rg.tags
}
