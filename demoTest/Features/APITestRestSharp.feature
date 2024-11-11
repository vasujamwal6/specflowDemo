Feature: APITestRestSharp

API Testing demo using Rest Sharp
@ApiTest
Scenario Outline: Verify object details recieved using ID
	Given User has an ID <ID> to request for <Endpoint>
    When User requests details using endpoint and ID <ID>
    Then User should recieve object details with name as <Name> and status code <StatusCode>

    Examples:
    |ID | Endpoint | StatusCode | Name | 
    |3  | objects  |  200       | Apple iPhone 12 Pro Max |
    |5  | objects  |  200       | Samsung  |


@ApiTest
Scenario Outline: Verify user is able to create new object using api
    
    Given User wants to create new object with parameters as <Color> and <capacityGB>
    When User creates new object with endpoint <Endpoint> having <Color> and <capacityGB>
    Then User verifies new object is created with color <Color>, capacity GB and status <StatusCode>
     Examples:
    | Endpoint | StatusCode | Color | capacityGB | 
    | objects  |  200       | Blue | 128          |
    | objects  |  200       | Grey | 256          |
