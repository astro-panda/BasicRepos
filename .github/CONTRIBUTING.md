## Getting Started
---

First thing to do is to install the BasicRepos NuGet package for a .NET project you're working on. Try working with the services a bit and get a feel for how the APIs in this project are used.

Next, if you'd still like to contribute take a look at some of the issues we have open. If you see something you're interested in giving a go on, make a comment and ask us if it's still relevant and to double check that no one else has already started on that work. It's also a good idea to go ahead and take that time to get an idea for the direction we're headed with the project overall so that you can be sure that your solution aligns well.


## Issues
---

When you find a problem or see a gap in the features, go ahead and post a descriptive issue. 

For bugs, make sure to include:
- The problem you're experiencing
- The target framework of the consuming project you're working in
- The OS type and version
- The behavior you are hoping to see

For feature requests, make sure to include:
- A high level overview of the feature and the value you see that it offers to the project
- A proposal of how you think the feature would be used or implemented

These are just guidelines to help us all better communicate, so don't stress about the bullet points. The key thing is to make sure you're descriptive enough that we understand what you need.

## Pull Requests
---

When making a Pull Request, be sure to be descriptive about what you're doing in the commits you've made. 

We might not be able to get to your PR immediately, so please be patient with us. When we have questions, please work with us to make sure we're all on the same page. That's the best way to nurture a PR into a merged commit.

If you're changing functionality in anyway, be sure to add unit tests that provide coverage of your changes to prove that they work.

In some cases we might not accept your Pull Request but we'll try to make it clear why we did not, in those cases. This is also why it's best practice to start a conversation _before_ starting work on a PR, because you'll start out with a clear picture and we'll be more aware as a team.


## Developing with BasicRepos
---
For proofing an API the best way to start is to make unit tests! We've already got a testing project available so you can make your tests there.

If you need to test the full integration of a feature, go ahead and make a stubbed .NET project to run with BasicRepos as a dependency.

When making changes and proving them in another project, you should probably consider making a local NuGet cache and adding it to your NuGet sources for the project. [More information about that here](https://learn.microsoft.com/en-us/nuget/hosting-packages/local-feeds).


Most of all thanks for building Basic Repos with us!