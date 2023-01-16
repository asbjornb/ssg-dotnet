# ssg-dotnet

An attempt at a small static site generator in C# to render my foam notes and some other static assets.

Some planned features:

* Convert markdown to html
* Place output as index.html files in folders to allow /something/ urls
* Use html templates to allow switching layouts for all pages or a subset at a time
* Allow nesting templates to avoid duplication of common items like menu's
* Support markdown linking and backlink gathering for Foam notes
* Add a configuration file to allow some simple configuration
* Potentially add support for gathering input files from a github repo rather than local filesystem and maybe publishing directly to cloudflare as well
* Potentially add watch functionality for faster turnaround when developing locally
