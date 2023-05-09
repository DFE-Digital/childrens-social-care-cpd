Feature:  External Links Tests
 Scenario: ASYE has a external link  to 'https://www.skillsforcare.org.uk/Regulated-professions/Social-work/ASYE/ASYE.aspx'
   Given a user has clicked on 'Practitioners'
    When user clicks on ' Assessed and supported year in employment'
    Then the page has a external link to 'https://www.skillsforcare.org.uk/Regulated-professions/Social-work/ASYE/ASYE.aspx'

 Scenario: Post qualifying standards for child and family practitioners has a external link  to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
   Given a user has clicked on 'Practitioners'
    When user clicks on ' Assessed and supported year in employment'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: Social work post has a external link  to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: FrontLine link has a program to 'https://thefrontline.org.uk/pathways-programme/'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/'

 Scenario: FrontLine pathway 1 programm link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-1/'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-1/'


 Scenario: FrontLine href link  to 'https://thefrontline.org.uk/pathways-programme/'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/'

Scenario: Social work post in pathway 2 has a  link  to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: FrontLine pathway 2 link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-2/'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-2/'

 Scenario: FrontLine network leaderslink to 'https://thefrontline.org.uk/fellowship/'
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://thefrontline.org.uk/fellowship/'

 Scenario: Social work link Managers link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: Mangers pathway 2 leaders fellowship link to 'https://thefrontline.org.uk/fellowship/'
    Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://thefrontline.org.uk/fellowship/'

 Scenario: Mangers pathway 2 programm link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-2/'
    Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 2: middle managers'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/apply-now/pathway-2/'

 Scenario: Social work link in senior managers link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Senior managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: Senior Manger network of leaders link 'https://thefrontline.org.uk/fellowship/'
    Given a user has clicked on 'Senior managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the page has a external link to 'https://thefrontline.org.uk/fellowship/'

 Scenario: Senior Manger pathway 4 social link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Senior managers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: Senior Manger pathway 4 frontline link to 'https://thefrontline.org.uk/pathways-programme/'
    Given a user has clicked on 'Senior managers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the page has a external link to 'https://thefrontline.org.uk/pathways-programme/'

 Scenario: Senior Manger pathway 4 fellow leaders link to 'https://thefrontline.org.uk/fellowship/'
    Given a user has clicked on 'Senior managers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the page has a external link to 'https://thefrontline.org.uk/fellowship/'

 Scenario: Upon: aspirant directors programme  link to 'https://uponleaders.co.uk/aspirant-directors-programme/'
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: aspirant directors programme'
    Then the page has a external link to 'https://uponleaders.co.uk/aspirant-directors-programme/'

 Scenario: Upon: aspirant directors programme social link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: aspirant directors programme'
    Then the page has a external link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'

 Scenario: Upon: new directors programme link to 'https://uponleaders.co.uk/new-directors-programme/'
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: new directors programme'
    Then the page has a external link to 'https://uponleaders.co.uk/new-directors-programme/'

 Scenario: Upon: new directors programme social link to 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: new directors programme'
    Then the page has a external link to '123'