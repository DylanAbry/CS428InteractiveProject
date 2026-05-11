
Assignment 7
Post-Mortem Report
5/11/26


# Brief overview of our two pillars and achievement
### Our project focused on two main technical pillars: animation and physics.
* Our first pillar was animation, which helped make the gameplay feel more interactive and polished. We implemented animations for the main player character, including idle, running, and jumping states. We also animated environmental obstacles, such as the swinging ninja star obstacle and the rotating totem bar obstacle, to create more dynamic gameplay and improve player immersion.
* Our second pillar was physics, which was essential for creating realistic player and environment interactions. We used rigidbody physics on the player to allow natural movement and collisions within the environment. Colliders were added to nearly all obstacles to ensure proper interaction and gameplay mechanics. We also implemented physics materials, such as bouncy surfaces on the pusher obstacles, which made it easier for players to be knocked off the map. Additionally, the ball pit obstacle used force and launch factors on each ball prefab so that collisions would scatter the balls realistically, mimicking the behavior of an actual ball pit.


# Playtest Resolution: 
### Detail exactly how you addressed the feedback and action items from your Assignment 6 user study.
* Based on feedback from our Assignment 6 user study, we made several major improvements to the game’s functionality, visuals, and overall player experience.
* One of the biggest updates was a complete UI overhaul. We redesigned and implemented the instruction, timer, pause, and win panels to make navigation and gameplay information much clearer for players. We also created a new title screen design and added a credits panel that players can access before starting the game.
* We also fully implemented the game’s audio systems. This included background music, obstacle and interaction sound effects, and voice lines from Sensei. Adding audio helped improve immersion and gave players clearer feedback during gameplay.
* Another major improvement was expanding the Zen Garden environment. During playtesting, the environment initially felt too empty, so we added water features, trees, bushes, rocks, lanterns, and statues to create a more peaceful and visually engaging space for players to explore before beginning the obstacle course.
* We additionally refined and polished the game’s core gameplay logic. We fixed issues where players could skip checkpoints, break menu functionality, or improperly interact with pause and credits systems. These fixes made the gameplay flow more stable and prevented unintended interactions.
* Finally, we made major memory optimizations to improve performance. Our original build size was nearly 2.7 GB due to the heavy use of 4K textures. After analyzing the build report, we reduced most textures to 1K resolution, which lowered the game size to approximately 0.5 GB while still maintaining visual quality. This optimization significantly improved the game’s efficiency and usability.


# Technical Post-Mortem: 
### What were the biggest architectural or engineering challenges you faced? Did you have to pivot or cut major features from your original Assignment 2 MVP definition? Why?
* **Architectural challenge:** asset management and storage optimization
   * Our original environment used many 4K textures, which dramatically increased our project size and slowed down build times. We had to downscale all textures from 4K to 1K. Since we just downloaded art asset packs from online we had to manually reorganize and only keep the materials and mesh files that were used in the project. 
* **Engineering challenge:** performance and physics stability
   * Our ball pit originally had over 1,000 physics balls, each with its own collider and texture. The game started lagging and glitching because the Unity physics engine was trying to calculate too many collisions at once. We optimized it to spawn around 800 balls, and tweaked some physics settings and collision layers, which made everything run much smoother. 
* We were able to successfully implement everything from our primary MVP and even went beyond. 


#AI Tool Evaluation:
###Reflect on your use of AI tools (generation, coding, etc.) throughout the semester. Did they speed up your workflow, or did debugging AI hallucinations cost you time?
* We used AI to help clarify coding issues, troubleshoot errors, and explain technical concepts when we got stuck during development. This saved time compared to searching through documentation or forums for every issue. We also used AI tools during the design process to help transform messy and unorganized ideas into clearer interface concepts and information architecture. This was especially useful for visualizing gameplay ideas and organizing our design decisions into more structured plans.
