package com.parse.starter;

/**
 * Created by rainierababao on 6/7/15.
 */
public class DodgeGame implements Game {

    // position of monster
    private int position;
    // energy level of monster
    private int energyLevel;
    // current monster
    private Monster m;

    public DodgeGame(Monster m) {
        this.m = m;
        System.out.println("MatchGame instantiated");
    }

    public void run() {
        while (m.getEnergyLevel() != 100) {
            // get current energyLevel of monster
            energyLevel = m.getEnergyLevel();
            // get current position of monster
            position = m.getPositionX();
            // now lets play the game!
            consume(pickFood());
            //System.out.println("Monster energy: " + currentMonster.getEnergyLevel());
            //System.out.println("Monster position: " + currentMonster.getPositionX());
        }
    }

    public static Food pickFood() {
        // pick food from random array of food objects
        Food[] consumeFood = {new Food("banana", 25, true), new Food("corn", 25, true), new Food("cupcake", 25, false)};
        return consumeFood[(int) (Math.random() * 3)];
    }

    // will the monster consume this food??
    public void consume (Food currFood) {

        // is the monster in the same place as the food
        if (currFood.getPositionX() == position) {
            if (currFood.getHealth()) {
                energyLevel+=currFood.getEnergyOfFood();
            } else {
                energyLevel-=currFood.getEnergyOfFood();
            }
        }
        m.setEnergyLevel(energyLevel);
        m.setPositionX((int) (Math.random() * 4));

    }
}
