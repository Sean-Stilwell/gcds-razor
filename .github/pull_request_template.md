## Summary

<!-- Describe what changed and why. Include any important behavior or compatibility changes. -->

## Related issues

<!-- Use "Closes #123" to automatically close an issue when this pull request is merged. -->

## Validation

<!-- Check the commands you ran and add any other manual testing below. -->

- [ ] `dotnet build GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj -c Release`
- [ ] `dotnet test --project GcdsWrapper.Blazor.Tests/GcdsWrapper.Blazor.Tests.csproj`
- [ ] `dotnet format GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj --verify-no-changes`
- [ ] Tested in a consuming Blazor app or the documentation demo, if applicable

Additional validation:

<!-- Describe relevant scenarios, browsers, assistive technology, or devices tested. -->

## Screenshots or example

<!-- For visible changes, include before/after screenshots or a small consuming-app example. Remove this section if it does not apply. -->

## GCDS impact

<!-- Note any upstream GCDS version change, affected components, and relevant changelog entries. Write "None" if this does not apply. -->

## Checklist

- [ ] Tests cover the changed behavior where appropriate
- [ ] Public API or usage documentation is updated where appropriate
- [ ] Generated `bin/` and `obj/` files are not included
- [ ] The change is focused and ready for review
