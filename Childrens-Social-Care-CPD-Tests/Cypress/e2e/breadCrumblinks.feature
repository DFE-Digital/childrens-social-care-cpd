Feature:  BreadCrumb links Tests
 Scenario: BreadCrumb link Tests for practioner home page redirect
  Given a user has clicked on 'Practitioners'
    Then the user clicks on 'Home' link should redirect to ''

 Scenario: BreadCrumb link redirects to practioner page 
  Given a user has clicked on 'Practitioners'
  When user clicks on ' Assessed and supported year in employment'
    Then the user clicks on 'Practitioners' link should redirect to '/?pageName=Practitioners&pageType=SubNavAlternate&sendingPage=Home&sendingPageType=Master'

Scenario: BreadCrumb link sub heading redirects to practioner page
  Given a user has clicked on 'Practitioners'
  When user clicks on sub heading
    Then the user clicks on 'Practitioners' link should redirect to '/?pageName=Practitioners&pageType=SubNavAlternate&sendingPage=Home&sendingPageType=Master'

 Scenario: BreadCrumb link Tests for Experienced practioner home page redirect
  Given a user has clicked on 'Experienced practitioners'
    Then the user clicks on 'Home' link should redirect to ''

 Scenario: BreadCrumb link Tests redirects to experienced practioner page from pathway 1
     Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the user clicks on 'Experienced practitioners' link should redirect to 'Experienced%20practitioners&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

    Scenario: BreadCrumb link Tests redirects to experienced practioner page from Pathway 2
     Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the user clicks on 'Experienced practitioners' link should redirect to 'Experienced%20practitioners&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

    Scenario: BreadCrumb link Tests redirects to managers page from Pathway 2
     Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 2: middle managers'
    Then the user clicks on 'Managers' link should redirect to '/?pageName=Managers&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

     Scenario: BreadCrumb link Tests redirects to managers page from Pathway 3 head service
     Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the user clicks on 'Managers' link should redirect to '/?pageName=Managers&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

     Scenario: BreadCrumb link Tests redirects to Senior managers page from Pathway 3 head service
     Given a user has clicked on 'Senior managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the user clicks on 'Senior managers' link should redirect to '/?pageName=Senior%20managers&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

     Scenario: BreadCrumb link Tests redirects to Senior managers page from Pathway 4 practice leader
     Given a user has clicked on 'Senior managers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the user clicks on 'Senior managers' link should redirect to '/?pageName=Senior%20managers&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

    Scenario: BreadCrumb link Tests redirects to leaders page from Pathway 4 practice leader
     Given a user has clicked on 'Leaders'
    When user clicks on ' Pathway 4: practice leaders'
    Then the user clicks on 'Leaders' link should redirect to '/?pageName=Leaders&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

    Scenario: BreadCrumb link Tests redirects to leaders page from Upon: aspirant directors programme
     Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: aspirant directors programme'
    Then the user clicks on 'Leaders' link should redirect to '/?pageName=Leaders&pageType=Programmes&sendingPage=Home&sendingPageType=Master'

     Scenario: BreadCrumb link Tests redirects to leaders page from Upon: new directors programme
     Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: new directors programme'
    Then the user clicks on 'Leaders' link should redirect to '/?pageName=Leaders&pageType=Programmes&sendingPage=Home&sendingPageType=Master'









