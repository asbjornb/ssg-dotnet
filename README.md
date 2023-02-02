# ssg-dotnet

An attempt at a small static site generator in C# to render my foam notes and some other static assets.

Some planned features:

- [x] Convert markdown to html
- [x] Place output as index.html files in folders to allow /something/ urls
- [x] Use html templates to allow switching layouts for all pages or a subset at a time
- [ ] Allow nesting templates to avoid duplication of common items like menu's
- [x] Support markdown linking and backlink gathering for Foam notes
- [ ] Add a configuration file to allow some simple configuration
- [ ] Support for gathering input files from a github repo rather than local filesystem
- [ ] Support for publishing directly to cloudflare? Or just push output to the cloudflare tracked repo?
- [ ] Potentially add watch functionality for faster turnaround when developing locally
- [ ] Add support for ignoring files. Probably a custom file is easiest but support for ignoring .gitignore would be a neat addition as well

Some missing stuff in current markdown to html parsing - not all of those necessarily needed, but here to keep track:

- [ ] Pipe tables
- [ ] Auto parse email addresses/urls
- [ ] Should notes be prefixed with "/notes/" when generating internal links?
