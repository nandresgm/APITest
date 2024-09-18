Feature: Country

This feature tests the REST API for retrieving country details. 
It includes scenarios to validate the response structure, 
check specific fields like country names and capitals, 
and handle various data-driven test cases and error responses.

@API
Scenario: Validate Successful Response for US
	When I call the API with country code "US"
	Then the response status code should be 200
	And the response should contain the country name "United States"

@API
Scenario: Validate Response Structure for CA
	When I call the API with country code "CA"
	Then the response status code should be 200
	And the response structure should include the following keys
		| Key     |
		| name    |
		| capital |
		| region  |

@API
Scenario Outline: Validate Capital for Multiple Countries
	When I call the API with country code "<CountryCode>"
	Then the response status code should be 200
	And the capital should be "<Capital>"

Examples:
	| CountryCode | Capital |
	| GB          | London  |
	| FR          | Paris   |
	| JP          | Tokyo   |

@API
Scenario: Validate Invalid Country Code
	When I call the API with country code "XYZ"
	Then the response status code should be 404
	And the response should contain an error message "Not Found"