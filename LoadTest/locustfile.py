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
        self.client.get("?pageName=Social%20Workers%20-ASYE&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master", name="newly_qualified/detail")

    @task
    def practitioners(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Improve%20your%20social%20work%20practice&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master", name="practitioners/detail")

    @task
    def supervisors_pathway1(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Practice%20supervisors&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors/pathway1")
        self.client.get("?pageName=Pathway1-practice%20supervisors&pageType=PathwayDetails&sendingPage=Practice%20supervisors&sendingPageType=Programmes", name="supervisors/pathway1/detail")
    
    @task
    def supervisors_pathway2(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Practice%20supervisors&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors/pathway2")
        self.client.get("?pageName=Practice%20supervisors%20Pathway%202%3A%20middle%20managers&pageType=PathwayDetails&sendingPage=Practice%20supervisors&sendingPageType=Programmes", name="supervisors/pathway2/detail")

    @task
    def supervisors_pathway3(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Middle%20managers%20and%20heads%20of%20service&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors/pathway3")
        self.client.get("?pageName=Pathway%203-heads%20of%20service&pageType=PathwayDetails&sendingPage=Middle%20managers%20and%20heads%20of%20service&sendingPageType=Programmes", name="supervisors/pathway3/detail")

    @task
    def supervisors_pathway4(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Middle%20managers%20and%20heads%20of%20service&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="supervisors/pathway4")
        self.client.get("?pageName=Pathway4-practice%20leaders&pageType=PathwayDetails&sendingPage=Middle%20managers%20and%20heads%20of%20service&sendingPageType=Programmes", name="supervisors/pathway4/detail")

    @task
    def senior_managers_aspiring(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers/pathway/aspiring")
        self.client.get("?pageName=Upon-aspiring%20directors&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/aspiring/detail")

    @task
    def senior_managers_new_directors(self):
        self.client.get("/", name="home")
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers/pathway/new-directors")
        self.client.get("?pageName=Upon-new%20directors&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/new-directors/detail")

    @task
    def senior_managers_pathway4(self):
        self.client.get("/", name="home", verify=-False)
        self.client.get("?pageName=Senior%20managers%20and%20leaders&pageType=Programmes&sendingPage=HomePage&sendingPageType=Master", name="senior-managers/pathway/pathway4")
        self.client.get("?pageName=Pathway4-practice%20leaders&pageType=PathwayDetails&sendingPage=Senior%20managers%20and%20leaders&sendingPageType=Programmes", name="senior-managers/pathway/pathway4/detail")