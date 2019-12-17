**Summary**  
Project contains two applications, a producer which produces 1000 messages and puts to the topic named `test-topic`, and consumers which consume messages from `test-topic` and print to the console. Each message is printed after 500 milliseconds which makes consumption is slower than production.  

**How to run**
1. Start Kafka (and therefore Zookeeper) by running following command on the project root   
`docker-compose up -d`
This will spin up two container named helloserverless-zookeper and helloserverless-kafka
2. Add following line to your hosts file in order to reach the Kafka's advertised address 
`127.0.0.1      kafka`
3. In order to partition the topic enter the Kafka container's bash run the following command  
`docker exec -it helloserverless-kafka bin/bash`
4. While on the bash of the Kafka container run the following command to alter the test topic's partition to 3  
`kafka-topics.sh --alter --zookeeper zookeeper:2181 --topic test-topic --partitions 3`  
Topics can be inspected by running  
`kafka-topics.sh --describe --zookeeper zookeeper:2181 --topic test-topic`
5. Type `exit` to exit from bash
6. Build consumer project by typing following command inside `Kafka.Consumer` directory  
`dotnet build -o out .`
7. Run as many consumers as needed (if the number exceeds partition count, some of the consumers would not get messages) by typing following command inside `Kafka.Consumer/out` directory  
`dotnet Kafka.Consumer.dll`
8. Build producer project by typing following command inside `Kafka.Producer` directory  
`dotnet build -o out .`
9. Run a producer by typing following command inside `Kafka.Producer/out` directory  
`dotnet Kafka.Producer.dll`