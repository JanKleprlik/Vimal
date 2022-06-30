package com.jetbrains.internship


println("Hello, world from Kotlin!")


for (i in 1..10) {
    println("Hello, world from Kotlin $i times.")
    Thread.sleep(100)
}

printCurrentTime()
Thread.sleep(1000)
printCurrentTime()


for (i in 1..10) {
    print("$i")
    Thread.sleep(100)
}



//print current time formated
fun printCurrentTime() {
    val currentTime = System.currentTimeMillis()
    val formatter = java.text.SimpleDateFormat("HH:mm:ss")
    val date = java.util.Date(currentTime)
    println("Current time is " + formatter.format(date))
}

//as? as break class continue do if else false true fun for
//package !in in null this try typealias typeof var val
//  i++ i-- object when while { } ( ) [ ]