### Week 12 – CRUD (Vertical Slice: Quests)

This week I implemented a full CRUD slice for Quests using async EF Core, a service layer, and validation on create/edit. I added a QuestsController with Index (list), Details (read), Create, and Edit actions. The controller calls IQuestService, which performs async data access with ToListAsync, FirstOrDefaultAsync, and SaveChangesAsync. For forms, I used a QuestEditViewModel with data annotations so invalid input (missing title, missing game/player) shows validation messages and blocks the POST. The Create and Edit views include asp-validation-summary and asp-validation-for to surface errors cleanly.

I kept the UI simple: Index lists quests with game, player, due date, and a quick link to Details/Edit. Create and Edit use dropdowns populated from Game and Player via the service. The service also logs a few basic actions so I can confirm the flow. This slice is intentionally narrow but complete end-to-end, which sets up Weeks 13–14 to add diagnostics and logging around the same path without changing the behavior.

Acceptance Criteria
- Async data access: ToListAsync / FirstOrDefaultAsync / SaveChangesAsync
- Validation feedback appears on Create/Edit when inputs are invalid
- List + Detail read paths work
- At least one write action works (Create and Edit implemented)

Evidence
- Controller, services, view models, and views are in the repository under their folders
- Screenshots of Create validation, Index list, and Details page (in /docs/week12/)
- Commits linked from this section

Test Plan
- Try creating a quest without a Title; expect validation error
- Create a valid quest; expect redirect to Details and success message
- Edit the quest’s IsCompleted or DueDate; expect update to persist
- Search in Index to confirm list filtering still works
