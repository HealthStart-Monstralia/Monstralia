package com.parse.starter;

public class ColorQuiz implements Game {

    Monster m;

    public ColorQuiz() {
        run();
    }

    public void run() {
        // simple demo for hackathon- will select values to populate monster from hard-coded arrays
        String color = colorFind();
        String name = nameFind();
        String emotion = emotionFind();
        int energyLevel = energyLevelFind();
        String bodyPart = bodyPartFind();
        Monster m = new Monster (name, energyLevel, emotion, color, bodyPart, positionFind(), positionFind());
        // printStats();
    }

    public void printStats() {
        System.out.println("Monster name: " + m.getName());
        System.out.println("Monster color: " + m.getColor());
        System.out.println("Monster emotion: " + m.getEmotion());
        System.out.println("Monster body part: " + m.getBodyPart());
        System.out.println("Monster energy level: " + m.getEnergyLevel());
        System.out.println("Monster positionX: " + m.getPositionX());
        System.out.println("Monster positionY: " + m.getPositionY());
    }

    public static String colorFind () {
        // random array
        String[] colors = {"green", "blue", "red", "yellow"};
        return colors[(int) (Math.random() * 4)];

    }

    public static int energyLevelFind () {
        // will be initially 0
        return 0;
    }

    public static String nameFind () {
        String[] names = {"Lucy", "Max", "Bob", "Amelia"};
        return names[(int) (Math.random() * 4)];
    }

    public static String emotionFind () {
        String[] emotions = {"Sad", "Happy"};
        return emotions[(int) (Math.random() * 2)];
    }

    public static String bodyPartFind () {
        String[] emotions = {"Brain", "Muscle", "Bones", "Digestive"};
        return emotions[(int) (Math.random() * 3)];
    }

    public static int positionFind () {
        return (int) (Math.random() * 4);
    }

    public Monster getMonster() {
        return m;
    }

}
