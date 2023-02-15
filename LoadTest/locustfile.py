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
    def newly_qualified_social_workers(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=%20Assessed%20and%20supported%20year%20in%20employment&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master", name="newly_qualified/detail")

    @task
    def practitioners(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Improve%20your%20social%20work%20practice&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master", name="practitioners/detail")

    @task
    def supervisors_pathway1(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Practice%20supervisors&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors")
        self.client.get("?pageName=Pathway%201%3A%20practice%20supervisors&pageType=PathwayDetails&sendingPage=Practice%20supervisors&sendingPageType=Programmes", name="supervisors/pathway1")
    
    @task
    def supervisors_pathway2(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Practice%20supervisors&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors")
        self.client.get("?pageName=Pathway%202%3A%20middle%20managers&pageType=PathwayDetails&sendingPage=Practice%20supervisors&sendingPageType=Programmes", name="supervisors/pathway2")

    @task
    def mm_pathway2(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Middle%20managers%20and%20heads%20of%20service&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="middlemanagers")
        self.client.get("?pageName=Pathway%202%3A%20middle%20managers&pageType=PathwayDetails&sendingPage=Middle%20managers%20and%20heads%20of%20service&sendingPageType=Programmes", name="middlemanagers/pathway2")

    @task
    def mm_pathway3(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Middle%20managers%20and%20heads%20of%20service&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="middlemanagers")
        self.client.get("?pageName=Pathway%203-heads%20of%20service&pageType=PathwayDetails&sendingPage=Middle%20managers%20and%20heads%20of%20service&sendingPageType=Programmes", name="middlemanagers/pathway3")

    @task
    def mm_pathway4(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Middle%20managers%20and%20heads%20of%20service&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="middlemanagers")
        self.client.get("?pageName=%20Pathway%204%3A%20practice%20leaders&pageType=PathwayDetails&sendingPage=Middle%20managers%20and%20heads%20of%20service&sendingPageType=Programmes", name="middlemanagers/pathway4")

    @task
    def senior_managers_aspiring(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers")
        self.client.get("?pageName=Upon%3A%20aspirant%20directors%20programme&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/aspiring")

    @task
    def senior_managers_new_directors(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers")
        self.client.get("?pageName=Upon%3A%20new%20directors%20programme&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/new-directors/detail")

    @task
    def senior_managers_pathway4(self):
        self.client.get("/", name="home", verify=-False)
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers")
        self.client.get("?pageName=%20Pathway%204%3A%20practice%20leaders&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/pathway4/detail")