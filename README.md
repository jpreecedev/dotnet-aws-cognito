# Using AWS Cognito from .NET Core 2.2 application

Run the application, use Postman to make the following requests;

## Create User

Make post request to `/api/register`. Example;

```json
{
  "Username": "test-user",
  "Password": "Ttest-passw32ord",
  "FamilyName": "Preece",
  "Gender": "Male",
  "GivenName": "Jon",
  "Name": "Jon Preece",
  "PhoneNumber": "+447955555555",
  "Email": "jon@jpreecedev.com"
}
```

## Sign user in

Assuming request is successful, make a request to `/api/signin`

```json
{
  "Username": "test-user",
  "Password": "Ttest-passw32ord"
}
```

## Call authorized endpoint

Call authorized endpoint `/api/values`, pass the `Authorization` header with the value `Bearer <id_token>`.
