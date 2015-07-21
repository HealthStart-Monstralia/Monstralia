package com.parse.starter;

// import com.parse.ParseClassName;
// import com.parse.ParseObject;

/*

The Monster class represents a Monster in the Monstralia game.
TODO: Setters and getters make direct queries to the Parse SDK.

 */

public class Monster {

    private int energyLevel;
    private String emotion;
    private String color;
    private String name;
    private String bodyPart;
    private int positionX;
    private int positionY;


    public Monster (String name, int energyLevel, String emotion, String color, String bodyPart, int positionX, int positionY) {

        this.energyLevel = energyLevel;
        this.name = name;
        this.color = color;
        this.emotion = emotion;
        this.bodyPart = bodyPart;
        this.positionX = positionX;
        this.positionY = positionY;
    }

    //getters
    public String getName() {
        return name;
    }

    public String getColor() {
        return color;
    }

    public String getEmotion() {
        return emotion;
    }

    public String getBodyPart() {
        return bodyPart;
    }

    public int getEnergyLevel() {
        return energyLevel;
    }

    public int getPositionX() {
        return positionX;
    }

    public int getPositionY() {
        return positionY;
    }


    //setters
    public void setName (String name) {
        this.name = name;
    }

    public void setColor (String color) {
        this.color = color;
    }

    public void setEnergyLevel (int energyLevel) {
        this.energyLevel = energyLevel;
    }

    public void setEmotion (String emotion) {
        this.emotion = emotion;
    }

    public void setPositionX (int positionX) {
        this.positionX = positionX;
    }

    public void setPositionY (int positionY) {
        this.positionY = positionY;
    }
}
