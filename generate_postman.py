import json
import uuid

def create_request(name, method, url_path, body=None, test_script=None):
    req = {
        "name": name,
        "event": [],
        "request": {
            "method": method,
            "header": [
                {
                    "key": "Authorization",
                    "value": "Bearer {{bearer_token}}",
                    "type": "text"
                }
            ],
            "url": {
                "raw": "{{base_url}}/" + url_path,
                "host": ["{{base_url}}"],
                "path": url_path.split("/")
            }
        },
        "response": []
    }
    
    if body:
        req["request"]["body"] = {
            "mode": "raw",
            "raw": json.dumps(body, indent=2),
            "options": {"raw": {"language": "json"}}
        }
        
    if test_script:
        req["event"].append({
            "listen": "test",
            "script": {
                "exec": test_script.split('\n'),
                "type": "text/javascript"
            }
        })
        
    return req

def create_crud_folder(entity_name, path, post_body, put_body, id_var_name, post_tests=None):
    if not post_tests:
        post_tests = f"""var jsonData = pm.response.json();
if (jsonData && jsonData.data && jsonData.data.id) {{
    pm.collectionVariables.set("{id_var_name}", jsonData.data.id);
}}"""

    return {
        "name": f"CRUD - {entity_name}",
        "item": [
            create_request(f"1. Create {entity_name}", "POST", path, post_body, post_tests),
            create_request(f"2. Get All {entity_name}s", "GET", path),
            create_request(f"3. Get {entity_name} by ID", "GET", f"{path}/{{{{{id_var_name}}}}}"),
            create_request(f"4. Update {entity_name}", "PUT", f"{path}/{{{{{id_var_name}}}}}", put_body),
            create_request(f"5. Delete {entity_name}", "DELETE", f"{path}/{{{{{id_var_name}}}}}")
        ]
    }

collection = {
    "info": {
        "name": "TradeOffStackAPI - Full Integration CRUD Tests",
        "description": "Collection Postman permettant de tester le flux complet CRUD (Create, Read, Update, Delete) pour toutes les entités de l'API. Chaque création sauvegarde automatiquement l'ID généré pour l'utiliser dans les requêtes suivantes.",
        "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
    },
    "item": [],
    "variable": [
        {"key": "base_url", "value": "http://localhost:5000", "type": "string"},
        {"key": "bearer_token", "value": "", "type": "string"},
        {"key": "department_id", "value": "", "type": "string"},
        {"key": "user_id", "value": "", "type": "string"},
        {"key": "equipment_id", "value": "", "type": "string"},
        {"key": "reservation_id", "value": "", "type": "string"},
        {"key": "maintenance_id", "value": "", "type": "string"}
    ]
}

# 1. Auth Flow
auth_tests = """var jsonData = pm.response.json();
if (jsonData && jsonData.data && jsonData.data.token) {
    pm.collectionVariables.set("bearer_token", jsonData.data.token);
}"""

collection["item"].append({
    "name": "0. Authentication",
    "item": [
        {
            "name": "Register Admin User",
            "request": {
                "method": "POST",
                "header": [],
                "body": {
                    "mode": "raw",
                    "raw": json.dumps({"firstName": "Admin", "lastName": "Postman", "email": "admin.postman@tradeoffstack.com", "password": "Password123!"}, indent=2),
                    "options": {"raw": {"language": "json"}}
                },
                "url": {"raw": "{{base_url}}/api/Auth/register", "host": ["{{base_url}}"], "path": ["api", "Auth", "register"]}
            }
        },
        {
            "name": "Login (Get Token)",
            "event": [{"listen": "test", "script": {"exec": auth_tests.split('\n'), "type": "text/javascript"}}],
            "request": {
                "method": "POST",
                "header": [],
                "body": {
                    "mode": "raw",
                    "raw": json.dumps({"email": "admin.postman@tradeoffstack.com", "password": "Password123!"}, indent=2),
                    "options": {"raw": {"language": "json"}}
                },
                "url": {"raw": "{{base_url}}/api/Auth/login", "host": ["{{base_url}}"], "path": ["api", "Auth", "login"]}
            }
        }
    ]
})

# 2. Departments
collection["item"].append(create_crud_folder(
    "Department", 
    "api/Department", 
    {"name": "IT Department", "description": "Information Technology Services"},
    {"name": "IT & Infra", "description": "Information Technology and Infrastructure Services"},
    "department_id"
))

# 3. Users (Employee test account)
# Note: Registration is used for self sign-up, but Admin can create users directly or update roles. Let's just do a CRUD on user details if admin allows. Wait, UserController has Get, Put, Delete. We'll use the Admin account ID we logged in as for tests, or register another user and get its ID.
# Let's create a User via UserController. But UserController might not have POST (only Register). Let's check. 
# We'll just do a GET /api/User to find an ID or register a test user.
user_post_tests = """var jsonData = pm.response.json();
if (jsonData && jsonData.data && jsonData.data.user && jsonData.data.user.id) {
    pm.collectionVariables.set("user_id", jsonData.data.user.id);
}"""
collection["item"].append({
    "name": "CRUD - User",
    "item": [
        create_request("1. Create User (Register)", "POST", "api/Auth/register", {"firstName": "Test", "lastName": "Employee", "email": "test.emp@tradeoffstack.com", "password": "Password123!"}, user_post_tests),
        create_request("2. Get All Users", "GET", "api/User"),
        create_request("3. Get User by ID", "GET", "api/User/{{user_id}}"),
        create_request("4. Update User Role (Admin)", "PUT", "api/User/{{user_id}}/role", {"role": 1}),
        create_request("5. Delete User", "DELETE", "api/User/{{user_id}}")
    ]
})

# 4. Equipment
collection["item"].append(create_crud_folder(
    "Equipment", 
    "api/Equipment", 
    {"name": "Dell XPS 15", "serial_number": "SN-XPS-999", "status": 1, "category": 1, "price": 1999.99, "description": "Laptop for dev"},
    {"name": "Dell XPS 15", "serial_number": "SN-XPS-999", "status": 1, "category": 1, "price": 1899.99, "description": "Laptop for dev - Price reduced"},
    "equipment_id"
))

# 5. Reservation
collection["item"].append(create_crud_folder(
    "Reservation", 
    "api/Reservation", 
    {"equipment_id": "{{equipment_id}}", "user_id": "{{user_id}}", "start_date": "2026-06-10T10:00:00Z", "end_date": "2026-06-20T10:00:00Z", "notes": "Need this for remote work."},
    {"status": 1, "start_date": "2026-06-10T10:00:00Z", "end_date": "2026-06-25T10:00:00Z", "notes": "Extended reservation."},
    "reservation_id"
))

# 6. MaintenanceRequest
collection["item"].append(create_crud_folder(
    "MaintenanceRequest", 
    "api/MaintenanceRequest", 
    {"equipment_id": "{{equipment_id}}", "requested_by_id": "{{user_id}}", "description": "Screen flickering issue", "priority": 2},
    {"status": 1, "description": "Screen flickering issue - Parts ordered", "priority": 3},
    "maintenance_id"
))

with open('TradeOffStackAPI_Postman_Collection.json', 'w') as f:
    json.dump(collection, f, indent=2)

