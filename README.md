# Orca
Orca is a lightweight deployment runtime written on top
of the Youki container runtime. Built to bridge the gap
between virtual deployment and host deployment, Orca offers
a rich and intuitive deployment process that even newbies
can get around. Take a look below at the feature set that
Orca provides.

# Features

**Modularity** <br>
Orca is built to be modular and expandable. With convenient
support for plugin development, Orca can be expanded to
support whatever workflow that you need it to.

**Usability** <br>
Orca prioritizes being usable first. While applications such
as Docker or Kubernetes are built to be robust and powerful,
they can often be intimidating and complex. Orca is utility
first, aiming to be intuitive and workable out of the box

**Deployability** <br>
Docker and Kubernetes are wonderful tools, however, they primarily
target virtualized workloads. Orca aims to support virtualized, 
local, development, test, and production environments all in one. 
This can significantly increase the speed at which development can occur.

# Status
As of August 3, 2025, Orca is in active development. Currently the
CLI, Daemon, and subsequent libraries are all under heavy
iteration. The current focus is on pulling OCI bundles and building/mounting
container filesystems.

# Roadmap
- [ ] Wrap Youki, filesystem, and OCI management
- [ ] Expand communication tunnels
- [ ] Prettyify command line interface
- [ ] Implement extensive event loop
- [ ] Implement plugin loader
