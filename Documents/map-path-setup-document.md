# Create Path

Scriptname: `MapPathSO.cs`

1. Go to `Assets/ScriptableObjects/Paths`.
2. Create new folder with scene name.
3. `Create > CryptoQuest > Map > Map Path`.
     ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/aca7cea2-3d1e-484e-a1f9-d4b6ea648107)

4. Rename path according to naming convention.

# Path Naming

**Use `_` to replace the world `to`**
Example:

- Castle to World => Castle_World
- Village to Shop => Village_Shop

**Use `.` to replace `of` to specify the location**
Example:

- Village to Floor 1 of Chief House => Village_ChiefHouse.Floor1
- World to North Door of Castle => World_Castle.NorthDoor

**If transit from the same place but a different specified location => Place.Location1_Location2**
Example:

- Floor 1 of Chief House to Floor 2 of Chief House => ChiefHouse.Floor1_Floor2
- Floor 1 of Pulic House to Basement of Pulic House => PulicHouse.Floor1_Basement

# Setup Map Exit and Map Entrance

Map Exit will handle the transition from the current map to the next map.
Map Entrance will handle where the player will be placed when entering this current map.
**Set up Map Exit**

1. Navigate to `Assets/Prefabs/Maps`
   
   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/b5638a1c-52cc-470d-b81a-b041c1722a92)

2. Drag the `MapExit` prefab to the current map scene

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/01acdb53-9e81-4d4d-b4cd-90ffe4cd0de0)


3. Select `MapExit` on Hierarchy, in the inspector window, click `Place at Mouse Cursor` and place at desired position.

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/b873c1c0-96be-415e-8238-80de9828c4ff)


   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/0f67da97-67b6-48f9-9ab9-3c3cb333b3d6)

4. Choose the Next Scene to transit to
5. Choose Map path to match with the next scene MapEntrance's Map path

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/d70f3c22-42b2-4823-b8a7-77c9c059dd8a)



**Set up Map Entrance**

1. Navigate to `Assets/Prefabs/Maps`
2. Drag the `MapEntrance` prefab to the current map scene
3. Select `MapEntrance` on Hierarchy, in the inspector window, click `Place at Mouse Cursor` and place at desire position.
4. Choose Map path to match with the previous scene MapExit's Map path
5. Select Entrance Facing Direction

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/4a635bfb-465e-4714-817a-9aac01f51c0b)



**Note:
MapExit position and MapEntrance position must be separated by a distance (so that when entering the map, there will be no colliding with MapExit)**

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/6f94a2eb-41ef-4815-9c87-56902ceabff8)

   ![image](https://github.com/indigames/CryptoQuestClient/assets/102936052/1bf9b111-2c35-42d7-91b9-0a7663971f35)




