    function startGL(){

    // setup stage
    var bunnySchool = [];

    // create an new instance of a pixi stage
    var stage = new PIXI.Stage(0x66FF99);

    // create a renderer instance
    var renderer = PIXI.autoDetectRenderer(window.innerWidth, window.innerHeight);


    // add the renderer view element to the DOM
    document.body.appendChild(renderer.view);

    requestAnimFrame(animate);

    // create a texture from an image path
    var texture = PIXI.Texture.fromImage("pictures/mikpe.jpg");

    // creating fish
    var fish = new Fish(0.5, 0.5, 0.5, Math.random()*width, Math.random()*height);
    var school = new Generation(fish);



    for(var i = 0; i<20; ++i){
    // create a new Sprite using the texture
    var bunny = new PIXI.Sprite(texture);

    //Testcomment
    //TestComment2

    // center the sprites anchor point
    bunny.anchor.x = Math.random()*0.5;
    bunny.anchor.y = Math.random()*0.5;

    // move the sprite to the center of the screen
    bunny.position.x = Math.random()*width;
    bunny.position.y = Math.random()*height;


    bunnySchool.push(bunny);
    console.log(bunnySchool); 
    }

    for(var i = 0; i<bunnySchool.length; ++i){
        stage.addChild(bunnySchool[i]);
    }

    function animate() {
        requestAnimFrame(animate);

        // just for fun, let's rotate mr rabbit a little
        for(var i = 0; i<bunnySchool.length; ++i){
        bunnySchool[i].rotation += 0.1;
    }

    
        // bunnys.rotation += 0.1;
    

        // render the stage
        renderer.render(stage);
    }
}