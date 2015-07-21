# [CLICK HERE TO SEE THE APP PROTOTYPE](http://marvelapp.com/6badjg)

# Monstralia Backend Proof of Concept
Backend proof of concept for an Android app that gamifies good health habits for kids! Volunteer work for the HealthStart Foundation at ATX Hack for Change 2015 (June 5-7, 2015).

Heavily forked from the Parse SDK starter kit to make shipping faster

## What is Monstralia?
Gamifying good health habits for kids! Children ages 2-6 take control of a baby monster avatar and teach it to make healthy choices.

Check out the [project entry link](https://austin.brightidea.com/ATXHack4Change2015/D185)!

by Jeremy and Robin from HealthStart, volunteer hackers Ellen, David, Hayley, Rainier
* **Project Lead/Champion**: Jeremy, Robin
* **Graphic Design**: Jeremy, Ellen
* **UX Prototype**: David
* **Game logic/back-end proof of concept**: Hayley, Rainier

## Scope (Goals) for June 6, 2015:

### Frontend:

* Prototype/wireframe mockup
  * See awesome detailed diagram by David
  * Partial prototype, just the “Brain” adventure
* Game sprites/objects/backgrounds for prototype completed
* Background:
  * Trees, mountains, water, paths
  * Maze for maze game, sky, clouds
* Objects:
  * Healthy foods
  * Unhealthy foods
  * Platter for memory game
  * Energy bar
  * Monster (choose-a-monster options, “main prototype monster”)

### Backend:

* User/game session datastore using Parse SDK for Android and Parse Cloud
* Game logic with CRUD (create, read, update, delete) and logging capabilities
  * Dodging unhealthy foods (Food class)
  * Memory matching platters to foods (Random swapping of a List of Food objects)
  * Maze game (Food class, simple generated maze)
* **Completely separate from the frontend prototype for the purposes of shipping the prototype faster, but will minimally serve as a proof of concept for the backend**

Note: "strings.xml" containing the Parse API key we used for the project is in the .gitignore

### Built with ❤ in Austin, TX
