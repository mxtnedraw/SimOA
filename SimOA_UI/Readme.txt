1.此项目是学习asp.net MVC时为巩固知识掌握而做的，实现了管理系统必需的功能（登录，注销，用户管理，角色管理，权限管理等），在此基础上整合了之前做asp.net WebForm时做的一个注册页面，加入了修改密码，更新个人信息，工作流程管理（主要是报销和请假的审批和驳回，并在主页增加了待办事项快速入口）。主页加入了一个指针钟表（从网上找的的js前端代码，做了部分小修改）。
2.用到的技术和框架：EF,memcached,log4net,spring.net,jquery AJAX,easyUI,ligerUI等。
3.项目使用sqlite数据库存取数据（开始用的是SQL Server,后来觉得sqlite也足够用了，并且部署方便，正好可以学习一下EF操作sqlite,就适当的修改代码适配了sqlite）。
4.项目采用三层架构，数据访问层DAL和业务逻辑层BLL使用工厂模式实现低耦合，业务逻辑层BLL和表现层UI用的spring.net。
5.项目使用memcached替代session实现用户登录持久化（可设置7天免登录）。
6.项目使用log4net进行异常日志的记录。
7.(前端)主页利用ligerUI的仿windows桌面模板适当改造而成。各个功能模块通过点击桌面图标进入（ligerDialog和iframe），各模块功能页面使用easyUI的dataGrid模板构建。几乎所有请求都是使用AJAX完成的异步请求（部分请求如：检查用户名是否存在等功能必须同步请求得到结果，使用的是AJAX同步请求{async:false}）。
8.管理员账号：admin123,密码：123456。