or Week 13, the goal was to add a diagnostic health endpoint to the QuestBoard application and verify that the app can report its own readiness. This included creating a /healthz route and adding a real dependency check against the database. The endpoint needed to return simple JSON that helps with troubleshooting but does not expose any connection strings or other sensitive information. The /healthz endpoint now performs two checks:

General application health

Database connectivity check using the existing QuestBoardContext

If the application can successfully communicate with the database, the endpoint returns Healthy. If the database is unreachable, it returns Unhealthy along with a short diagnostic message.