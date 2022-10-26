variable "backend_address_pool_name" {
  default = "s185d01BackendPool"
}

variable "frontend_port_name" {
  default = "s185d01FrontendPort"
}

variable "frontend_ip_configuration_name" {
  default = "s185d01AGIPConfig"
}

variable "http_setting_name" {
  default = "s185d01HTTPsetting"
}

variable "listener_name" {
  default = "s185d01Listener"
}

variable "request_routing_rule_name" {
  default = "s185d01RoutingRule"
}

variable "redirect_configuration_name" {
  default = "s185d01RedirectConfig"
}

variable "tenant_id" {
  type      = string
  sensitive = true
}

variable "contentful_apikey" {
  type      = string
  default   = "<APIKEY>"
  sensitive = true
}

variable "contentful_space" {
  type      = string
  default   = "<SPACE>"
  sensitive = true
}
