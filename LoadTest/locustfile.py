from urllib3.exceptions import InsecureRequestWarning
from urllib3 import disable_warnings
from locust import HttpUser, task

disable_warnings(InsecureRequestWarning)

class QuickstartUser(HttpUser):
    def on_start(self):
        self.client.verify = False
        print(self.client.base_url)
        self.client.headers.update({"Referrer": self.client.base_url, "User-Agent": "Mozilla/5.0 (Windows NT 6.1"})

    @task    
    def explore_roles(self):
        self.client.get("/", name="home")
        self.client.get("/explore-roles", name="explore-roles")

    @task
    def explore_roles_nqsw(self):
        self.client.get("/", name="home")
        self.client.get("explore-roles", name="explore-roles")
        self.client.get("explore-roles/newly-qualified-social-worker", name="explore-roles/newly-qualified-social-worker")

    @task
    def explore_roles_sw(self):
        self.client.get("/", name="home")
        self.client.get("explore-roles", name="explore-roles")
        self.client.get("explore-roles/social-worker", name="explore-roles/social-worker")

    #@task
    #def explore_roles_sp(self):
    #    self.client.get("/", name="home")
    #    self.client.get("explore-roles", name="explore-roles")
    #    self.client.get("explore-roles/senior-practitioner", name="explore-roles/senior-practitioner")

    #@task
    #def explore_roles_ps(self):
    #    self.client.get("/", name="home")
    #    self.client.get("explore-roles", name="explore-roles")
    #    self.client.get("explore-roles/practise-supervisor", name="explore-roles/practise-supervisor")

    #@task
    #def pathway1(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("pathways/1", name="development-programmes/pathway1")
    
    #@task
    #def pathway2(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("pathways/2", name="development-programmes/pathway2")

    #@task
    #def pathway3(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("pathways/3", name="development-programmes/pathway3")

    #@task
    #def pathway4(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("pathways/4", name="development-programmes/pathway4")

    #@task
    #def upon_pathway1(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("upon-aspiring-directors", name="development-programmes/upon-aspiring-directors")

    #@task
    #def upon_pathway1(self):
    #    self.client.get("/", name="home")
    #    self.client.get("development-programmes", name="development-programmes")
    #    self.client.get("upon-new-directors", name="development-programmes/upon-new-directors")