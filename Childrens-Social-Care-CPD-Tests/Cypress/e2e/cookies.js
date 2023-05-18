import { Given, When, Then, Step, } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has arrived on the home page", () => {
    cy.visitcpdhomepage();
});


Then("the page URL ends with {string}", (path) => {
    cy.location().should((loc) => {
        expect(loc.search).to.eq(path)

    })
});

When("the {string} is selected", (id) => {
    cy.get('[datatestid="' + id + '"]').click();
});

Then("the page's title is {string}", (title) => {
    cy.get("h1").should("contain.text", title);
});

Then("the no option is checked", () => {
    cy.get('[datatestid="btn-accept"]').should("not.be.checked");
    cy.get('[datatestid="btn-reject"]').should("not.be.checked");
});

Then("the cookies page  has {string} option is checked", (id) => {
    cy.get('[datatestid="Cookies"]').click();
    cy.get('[datatestid="' + id + '"]').should("be.checked");
});