https://gettaurus.org/install/Installation/

docker run -it --rm -v C:\Users\ybust\source\repos\Docker\taurusloadtest\tmp\my-test:/bzt-configs blazemeter/taurus my-config.yml



execution:
- concurrency: 100
  ramp-up: 1m
  hold-for: 5m
  scenario: quick-test
scenarios:
  quick-test:
    requests:
    - http://spring-fibonacci-swagger2-110821-sandboxdeveloperframework.apps.arcsandbox.zezd.p1.openshiftapps.com/api/fibonacci?position=35



execution:
- concurrency: 100
  ramp-up: 1m
  hold-for: 5m
  scenario: quick-test

scenarios:
  quick-test:
    requests:
    - https://fibonacciapiyb-sandboxdeveloperframework.apps.arcsandbox.zezd.p1.openshiftapps.com/api/Fibonacci?position=35