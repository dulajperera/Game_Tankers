// This script uses the left and right arrow keys for steering, and the 
// up and down arrow keys for forward and backward movement. The rotation
// and movement rates are specified below.
// A gun can also be fired. The spawn point and the bullet object must be specified
// below.
var rotateSpeed = 90.0;
var speed = 5.0;
var fireInterval = 0.5;
var bulletSpeed = 20.0;
var spawnPoint : Transform;
var bulletObject : GameObject;
private var nextFire;

function Start() {
	nextFire = Time.time + fireInterval;
}

function Update() {
    var transAmount = speed * Time.deltaTime;
    var rotateAmount = rotateSpeed * Time.deltaTime;

    if (Input.GetKey("up")) {
        transform.Translate(0, 0, transAmount);
    }
    if (Input.GetKey("down")) {
        transform.Translate(0, 0, -transAmount);
    }
    if (Input.GetKey("left")) {
        transform.Rotate(0, -rotateAmount, 0);
    }
    if (Input.GetKey("right")) {
        transform.Rotate(0, rotateAmount, 0);
    }
	if (Input.GetButtonDown("Fire1") && Time.time > nextFire) {
		nextFire = Time.time + fireInterval;
		fire();
	}
}

function fire() {
	var bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation);
	bullet.GetComponent.<Rigidbody>().velocity = transform.forward * bulletSpeed;
}