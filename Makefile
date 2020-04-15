.PHONY: login build push pull run

all: login build push

build:
	docker build . -t magida/iot-project

push:
	docker push magida/iot-project

pull:
	docker pull magida/iot-project

run:
	docker-compose up -d && docker-compose logs --follow

rundocker:
	docker container run -p 80:3000 magida/iot-project

login:
	docker login

clean:
	docker-compose down
