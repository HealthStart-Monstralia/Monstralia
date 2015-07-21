package com.parse.starter;

public class Food {

	private String name;
	private int energyOfFood;
	private boolean healthy;
	private int positionX;
	private int positionY;

	public Food(String name, int energyOfFood, boolean healthy) {
		this.name = name;
		this.energyOfFood = energyOfFood;
		this.healthy = healthy;
		this.positionX = (int) (Math.random() * 4);
		this.positionY = (int) (Math.random() * 2);

	}

	//setters
	public void setEnergyOfFood (int energy) {
		this.energyOfFood = energy;
	}

	public void setHealth (boolean health) {
		this.healthy = health;
	}

	//getters
	public int getEnergyOfFood () {
		return energyOfFood;
	}

	public boolean getHealth () {
		return this.healthy;
	}

	public int getPositionX () {
		return this.positionX;
	}

	public int getPositionY () {
		return this.positionY;

	}

}
