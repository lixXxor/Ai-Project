//Initial class function. To instantiate, simply call "new Fish()" with desired arguments.
var Fish = function(initSpeed, initAngle,initTurnspeed, initX, initY){
	this.speed = initSpeed;
	this.angle = initAngle;
	this.turnspeed = initTurnspeed;
	this.position = {
		x: initX,
		y: initY
	}
	// alert("fish created");
}

//Syntax to create a member function of the class

/**GETTERS**/
Fish.prototype.getSpeed = function(){
	return this.speed;
}

Fish.prototype.getAngle = function(){
	return this.angle;
}

Fish.prototype.getPosition = function(){
	return this.position
}

Fish.prototype.getTurnspeed = function(){
	return this.turnspeed;
}

/**SETTERS**/
Fish.prototype.updatePosition = function(dt){
	this.position.x += Math.cos(this.angle)*this.speed*dt;
	this.position.y += Math.sin(this.angle)*this.speed*dt;
}

Fish.prototype.updateAngle = function(dt){
	if((this.angle + this.turnspeed*dt) < 360 && (this.angle + this.turnspeed*dt) > 0)
		this.angle += this.turnspeed*dt;
	else if((this.angle + this.turnspeed*dt) >= 360)
		this.angle += this.turnspeed*dt - 360;
	else if((this.angle + this.turnspeed*dt) <= 0)
		this.angle += this.turnspeed*dt + 360;
}

Fish.prototype.setSpeed = function(newSpeed){
	this.speed = newSpeed;
}

Fish.prototype.setAngle = function(newAngle){
	this.angle = newAngle;
}

Fish.prototype.setTurnspeed = function(newTurnspeed){
	this.turnspeed = newTurnspeed;
}
