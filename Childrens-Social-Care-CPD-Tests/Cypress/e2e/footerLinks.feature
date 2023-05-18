Feature:  Footer Links Tests
Scenario: Footer has link to Accessibility statement
   Given a user has arrived on the home page
   Then the 'Accessibility statement' has link to '/?pageName=AccessibilityStatement&pageType=PathwayDetails&sendingPage=Home&sendingPageType=Master'

Scenario: Footer has link to Cookies page
   Given a user has arrived on the home page
   Then the 'Cookies' has link to '/?pageName=ViewCookies&pageType=PathwayDetails&sendingPage=Home&sendingPageType=Master'

Scenario: Footer has link to Privacy policy page
    Given a user has arrived on the home page
    Then the 'Privacy policy' has link to '/?pageName=PrivacyPolicy&pageType=PathwayDetails&sendingPage=Home&sendingPageType=Master'

Scenario: Footer has link to Terms and conditions page
    Given a user has arrived on the home page
    Then the 'Terms and conditions' has link to '/?pageName=Terms and conditions&pageType=PathwayDetails&sendingPage=Home&sendingPageType=Master'

Scenario: Footer has link to Feedback page
   Given a user has arrived on the home page
   Then the 'Feedback' has link to ' https://qfreeaccountssjc1.az1.qualtrics.com/jfe/form/SV_eR5TfL8aVrAMZvw'