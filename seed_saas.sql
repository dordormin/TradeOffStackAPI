-- Seed SaaSProviders
INSERT INTO "SaaSProviders" ("Id", "Name", "WebsiteUrl", "Category", "ContactEmail", "CreatedAt") VALUES 
('11111111-1111-1111-1111-111111111111', 'Microsoft', 'https://microsoft.com', 'Productivity', 'billing@microsoft.com', CURRENT_TIMESTAMP),
('22222222-2222-2222-2222-222222222222', 'Figma', 'https://figma.com', 'Design', 'billing@figma.com', CURRENT_TIMESTAMP),
('33333333-3333-3333-3333-333333333333', 'Slack', 'https://slack.com', 'Communication', 'billing@slack.com', CURRENT_TIMESTAMP),
('44444444-4444-4444-4444-444444444444', 'Adobe', 'https://adobe.com', 'Design', 'billing@adobe.com', CURRENT_TIMESTAMP)
ON CONFLICT ("Id") DO NOTHING;

-- Seed SaaSSubscriptions
INSERT INTO "SaaSSubscriptions" ("Id", "ProviderId", "PlanName", "BillingCycle", "CostPerSeat", "TotalSeats", "RenewalDate", "Status", "CreatedAt") VALUES 
('55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', 'Microsoft 365 E5', 'Monthly', 35.00, 100, CURRENT_DATE + INTERVAL '15 days', 'Active', CURRENT_TIMESTAMP),
('66666666-6666-6666-6666-666666666666', '22222222-2222-2222-2222-222222222222', 'Figma Organization', 'Yearly', 45.00, 20, CURRENT_DATE + INTERVAL '1 year', 'Active', CURRENT_TIMESTAMP),
('77777777-7777-7777-7777-777777777777', '33333333-3333-3333-3333-333333333333', 'Slack Enterprise Grid', 'Monthly', 15.00, 150, CURRENT_DATE + INTERVAL '1 month', 'Active', CURRENT_TIMESTAMP),
('88888888-8888-8888-8888-888888888888', '44444444-4444-4444-4444-444444444444', 'Creative Cloud All Apps', 'Monthly', 80.00, 10, CURRENT_DATE + INTERVAL '20 days', 'Active', CURRENT_TIMESTAMP)
ON CONFLICT ("Id") DO NOTHING;

-- Seed SaaSAssignments (linking to a real user in tradeoffstack_core: 8f1937d7-7db4-499d-b8a9-fdad274f3015)
INSERT INTO "SaaSAssignments" ("Id", "SubscriptionId", "UserId", "AssignedDate", "LastLoginDate", "IsActive") VALUES 
(gen_random_uuid(), '55555555-5555-5555-5555-555555555555', '8f1937d7-7db4-499d-b8a9-fdad274f3015', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP - INTERVAL '1 day', true),
(gen_random_uuid(), '66666666-6666-6666-6666-666666666666', '8f1937d7-7db4-499d-b8a9-fdad274f3015', CURRENT_TIMESTAMP - INTERVAL '10 days', CURRENT_TIMESTAMP - INTERVAL '5 days', true),
(gen_random_uuid(), '77777777-7777-7777-7777-777777777777', '8f1937d7-7db4-499d-b8a9-fdad274f3015', CURRENT_TIMESTAMP - INTERVAL '30 days', CURRENT_TIMESTAMP - INTERVAL '2 hours', true)
ON CONFLICT ("Id") DO NOTHING;
