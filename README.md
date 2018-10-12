# WellFired.Direct.Upgrade
A source dump of our version upgrade code for uSequencer.

When migrating from one version of uSequencer to another or from uSequencer -> WellFired.Direct, it's sometimes needed to run this migration tool if we've done something silly such as change our Dll location, or moved files from Dll -> script or vice versa.

Using the tool is quite simple.

1. Make a project backup
2. Import the package from our release page : https://github.com/WellFiredDevelopment/WellFired.Direct.Upgrade/files/2471549/WellFired.Direct.Upgrade.unitypackage.zip into your existing project, before doing an upgrade.
3. Run the following menu item in Unity (Window -> WellFired -> .Direct -> Write Existing Data). This will dump a json file in the root of your project with guid and fileid mappings.
4. Update uSequencer or import WellFired.Direct into your unity project, you may or may not have missing script references all over your project now, meaning your sequences probably no longer work.
5. Run the following menu item in Unity (Window -> WellFired -> .Direct -> Update From Existing)
6. Wait, maybe go make yourself a cup of tea!

Once this process has finished, you should hopefully have the new version of uSequencer (or WellFired.Direct) up and running and all your old prefabs, scenes, etc should be working wonderfully!

Enjoy, and feel free to lift this code for you own purposes, it's in no way tied to WellFired products!
