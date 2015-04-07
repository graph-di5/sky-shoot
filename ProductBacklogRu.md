Список задач, которые надо будет сделать. Без сортировки.
Забытые задачки можно скидывать в комментарии ниже.


## Список ##

  * Масса AGameObject: должна учитываться в collision detector для подсчета импульса (самый просто вариант)
  * Отталкивающие взрывы и пули: вероятно лучше все будет реализовать через предыдущую задачу "Масса AGameObject"
  * Ум мобов: различные стратегии поведения мобов. каждая стратегия должна быть выделена в отдельную задачу
  * Генерация бонусов: изменить генерацию бонусов при смерти мобов (не для всех мобов должны генерироваться бонусы)
  * Игровая статистика: сохранение и загрузка статистики между играми (и между заходами игрока)
  * перки: зависит от "Игровая статистика"
  * Новая модель взрыва (что это?)
  * Новая модель огнемета:
    * разрастание пуль
    * не N пуль, а одна большая которая уменьшается в размерах при соприкосновении с объектом
  * изменение радиуса картинки при изменении радиуса объекта: сейчас есть реализация, но она неуниверсальна и включает не все объекты
  * функциональность "готовность к игре": в контракте, сервере и клиенте
  * кланы и clanwars
  * масса оружия влияет на скорость бега: зависит от "Масса AGameObject", скорее будет реализовано через разные классы игроков
  * умная фильтрация сообщений на клиенте: в очередном пакете на клиент могут попасть сообщения, которые по времени были раньше, чем последнее уже обработанное клиентом
  * void зоны (огонь на земле, облака ядовитого газа и т.д.)
  * новые объекты на арене: веченая задача по наполнению контентом
  * учет времени и пинга: сейчас реализован учет времени и пинга на клиенте. недостатки:
    * в очередном пакете на клиент могут попасть сообщения, которые по времени были раньше, чем последнее уже обработанное клиентом (см. "умная фильтрация сообщений на клиенте")
    * на сервере нету экстраполяции при обработке сообщений - учитывается момент времени, в который сообщение пришло на сервер, а не время, когда это событие произошло на клиенте