# КПО - Мини ДЗ №2. Веб-приложение для автоматизации бизнес-процессов зоопарка.
---

## Реализованный функционал:

### Добавить / удалить животное
Реализовано в:
- AnimalsController:

  - Метод CreateAnimal() - создание нового животного в системе
  - Метод DeleteAnimal() - удаление животного из системы

- InMemoryAnimalRepository - хранение и управление данными о животных

### Добавить / удалить вольер
Реализовано в:

- EnclosuresController:

  - Метод CreateEnclosure() - создание нового вольера в системе
  - Метод DeleteEnclosure() - удаление вольера из системы


- InMemoryEnclosureRepository - хранение и управление данными о вольерах

### Переместить животное между вольерами
Реализовано в:

- AnimalTransferService - сервис, отвечающий за логику перемещения животных с генерацией события AnimalMovedEvent
- AnimalsController:

  - Метод TransferAnimal() - перемещение животного через сервис AnimalTransferService


- EnclosuresController:

  - Метод AddAnimalToEnclosure() - добавление животного в вольер
  - Метод RemoveAnimalFromEnclosure() - удаление животного из вольера


### Просмотреть расписание кормления
Реализовано в:

- FeedingSchedulesController:

  - Метод GetFeedingSchedules() - получение всех расписаний кормления
  - Метод GetFeedingSchedule() - получение конкретного расписания по ID
  - Метод GetSchedulesByAnimal() - получение расписаний для конкретного животного
  - Метод GetUpcomingSchedules() - получение расписаний на заданный период времени


- InMemoryFeedingScheduleRepository - хранение и управление данными о расписаниях кормления

### Добавить новое кормление в расписание
Реализовано в:

- FeedingSchedulesController:

  - Метод CreateFeedingSchedule() - создание нового расписания кормления
  - Метод UpdateFeedingSchedule() - обновление существующего расписания
  - Метод MarkAsCompleted() - отметка о выполнении кормления
  - Метод ResetFeedingStatus() - сброс статуса кормления


- FeedingOrganizationService - сервис, отвечающий за логику кормления с генерацией события FeedingTimeEvent

### Просмотреть статистику зоопарка
Реализовано в:

- StatisticsController:

  - Метод GetBasicStatistics() - получение основной статистики
  - Метод GetDetailedStatistics() - получение расширенной статистики


- ZooStatisticsService - сервис для сбора и обработки статистики зоопарка
---

## Применение концепций Domain-Driven Design

### Entities

- Animal - доменная сущность с уникальным идентификатором, инкапсулирует состояние и поведение животного
- Enclosure - доменная сущность, представляющая вольер с коллекцией идентификаторов животных
- FeedingSchedule - доменная сущность для расписания кормления

### Value Objects

- Gender - перечисление для пола животного
- HealthStatus - перечисление для состояния здоровья
- EnclosureType - перечисление для типов вольеров

### Domain Events

- AnimalMovedEvent - событие при перемещении животного между вольерами
- FeedingTimeEvent - событие при наступлении времени кормления

### Application Services

- AnimalTransferService - реализует логику перемещения животных между вольерами
- FeedingOrganizationService - реализует логику организации кормлений
- ZooStatisticsService - реализует сбор и обработку статистики зоопарка

### Репозитории (Repositories)

- IAnimalRepository - интерфейс для доступа к хранилищу животных
- IEnclosureRepository - интерфейс для доступа к хранилищу вольеров
- IFeedingScheduleRepository - интерфейс для доступа к хранилищу расписаний кормления

---

## Применение принципов Clean Architecture

### Domain Layer

- Domain/Entities - доменные сущности (Animal, Enclosure, FeedingSchedule)
- Domain/Value Object - объекты-значения (Gender, HealthStatus, EnclosureType)
- Domain/Events - доменные события (AnimalMovedEvent, FeedingTimeEvent)

### Application Layer

- Application/Services - доменные сервисы:

  - AnimalTransferService - сервис для перемещения животных
  - FeedingOrganizationService - сервис для организации кормлений
  - ZooStatisticsService - сервис для сбора статистики



### Infrastructure Layer

- Infrastructure/Interfaces - интерфейсы репозиториев
- Infrastructure/Repositories - реализации репозиториев (In-Memory):

  - InMemoryAnimalRepository
  - InMemoryEnclosureRepository
  - InMemoryFeedingScheduleRepository



### Presentation Layer

- Presentation/Controllers - API контроллеры:

  - AnimalsController
  - EnclosuresController
  - FeedingSchedulesController
  - StatisticsController

---

## Тестирование

Проект был протестирован с помощью xUnit.

Покрытие 80%

<img width="362" alt="tests" src="https://github.com/user-attachments/assets/48fb7925-4495-4ad8-85b4-fde40a00d4cf" />

<img width="479" alt="coverage" src="https://github.com/user-attachments/assets/36ae8068-e2da-48c7-b4dd-85e903fcab7d" />

---

## Swagger

<img width="1321" alt="animals" src="https://github.com/user-attachments/assets/166b6a04-e097-4049-8da3-ea3b4213fde1" />

<img width="1311" alt="enclosures" src="https://github.com/user-attachments/assets/c2ec281c-38a9-4e21-aa82-1333a11a6ccc" />

<img width="1322" alt="FeedingSchedules" src="https://github.com/user-attachments/assets/fad7ab8d-32fd-40f9-9ca7-b802beb4a4ff" />

<img width="1315" alt="Statistics" src="https://github.com/user-attachments/assets/b4578406-4341-495d-91ad-5ab80205f200" />




