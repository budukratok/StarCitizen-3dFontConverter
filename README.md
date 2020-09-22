# Star Citizen Unity Tools

Repository for simple Unity3d-based tools for Star Citizen localization initiative.

**Process** component allows to:
  - Move mesh pivot point and calculate the letter mesh center
  - Rotate mesh vertices around point
  - Scale 3d model by moving vertices
  
**Save** component allows to save selected GameObject as dae. file.

It's quite important to have a **correct pivot position, scale and rotation**. You can use Russian font 3d model in SampleScene as a reference.
Keep in mind that all operations modifies scene object and cannot be canceled (maybe do this later =), so feel free to duplicate you objects in scene hierarchy to make backups.

Our pipeline:
 - Import 3d model, make sure all models scale are 1 1 1, add Process and Save components, switch to pivot/local:
![Pivot/local mode](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/PWVPEBKVBORHNXS.png)

 - Select required characters and add them to Process component
 
![Selected chars](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/BUODOOYSEVLVIBR.png)
![Chars list](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/UKASVIVDHADQEDO.png)

 - Use Russian font as a reference and perform move/rotate/scale operations to make correct pivot position and rotation:

![Process window](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/IVETDGDYADVALOE.png)
![Model with correct pivot rotation and position](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/IJODSHDWTKMSGEX.png)
![Model with correct pivot rotation and position](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/YUPPNPRPYKRMAPZ.png)

 - Export whole model to .dae:

![Save component](https://github.com/budukratok/StarCitizen-UnityTools/blob/master/ReadmeImages/IWMHJEVDXPFRHCL.png)

Output file will be generated at \DAEWorks folder. Easy breezy.


### References
* [linuxaged/ColladaExporter](https://github.com/linuxaged/ColladaExporter) - Collada exporter for Unity3d (was changed and fixed)
