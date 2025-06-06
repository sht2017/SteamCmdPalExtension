<div align="center">

# <img src="../../SteamCmdPalExtension/Assets/Designs/StoreLogo-Transparent.svg" width="96" height="96" />

<div align="left">

# SteamPal for Command Palette <br> [![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0) [![License](https://img.shields.io/github/license/sht2017/SteamCmdPalExtension)](../LICENSE) <br> [![Microsoft Store](https://get.microsoft.com/images/zh-cn%20light.svg)](https://apps.microsoft.com/detail/9ns40lt8g6v9?referrer=appbadge&mode=direct)

[English](../readme.md) | 简体中文 | [正體中文](zh_TW.md)

<div style="border: 1px solid #ccc; padding: 10px; margin-top: 10px; border-radius: 8px; background-color:rgb(253, 253, 253);">
  <strong>ⓘ 信息</strong>
  <br>
  启动本扩展后，Steam 进程将被强制终止。请尽量避免在游戏运行时启动或重新加载命令面板。
</div>
<div style="border: 1px solid #ccc; padding: 10px; margin-top: 10px; border-radius: 8px; background-color: #f3e6b2;">
  <strong>ⓘ 注意:</strong>
  <br>
  SteamPal for Command Palette 通过 TCP 协议调用 Chrome 开发者工具接口，与 Steam CEF 组件交互并获取数据。请注意，该扩展可能与采用相同技术的其他程序产生冲突。如果出现冲突，建议尝试手动设置调试端口。此外，由于 Steam 服务在 Windows 系统中具有较高权限级别，若您无法确保系统环境可信，本地通过 TCP 进行 CEF 调试可能存在账号遭受黑客攻击的风险（之前试过通过管道来实现调试，但 steamwebhelper 在接受到正确管道句柄的情况下无法产生有效响应，以后研究清楚了可能会实现）。
</div>
<div style="border: 1px solid #ccc; padding: 10px; margin-top: 10px; border-radius: 8px; background-color: #cb514e; color: white;">
  <strong>ⓘ 重要</strong>
  <br>
  受上游缺陷影响（<a style="color: lightblue;" href="https://github.com/microsoft/PowerToys/issues/39837">microsoft/PowerToys#39837</a>），您可能会观察到使用本扩展时出现内存泄漏现象。目前暂时无法解决。
</div><br>

SteamPal 是一款 Powertoys 命令面板扩展，让你能直接在命令面板中快速搜索、启动和管理 Steam 游戏及应用程序。该扩展利用 Steam 客户端的调试接口，实时获取已安装游戏信息、游玩时长等数据，并能通过单一指令启动游戏。

![Preview](../assets/preview.png)

## 目录

- [功能特性](#功能特性)
- [安装指南](#安装指南)
  - [源码编译（开发模式）](#源码编译开发模式)
- [使用说明](#使用说明)
  - [搜索游戏](#搜索游戏)
  - [启动游戏](#启动游戏)
  - [查看游戏属性](#查看游戏属性)
  - [搜索 Steam 商店](#搜索-steam-商店)
- [设置选项](#设置选项)
- [贡献指南](#贡献指南)
- [开源许可](#开源许可)

## 功能特性

- **快速搜索 Steam 游戏**：通过在命令面板输入名称，即时检索 Steam 库中的游戏。
- **一键启动游戏**：直接从搜索结果启动已安装的 Steam 游戏。
- **查看游戏详情**：显示游戏关键信息，包括游玩时长、最后启动时间及游戏类型。
- **访问游戏属性**：为选定游戏打开 Steam 属性设置窗口。
- **搜索 Steam 商店**：当本地库未找到游戏时，可快速跳转 Steam 商店搜索。
- **实时数据同步**：通过 Steam 客户端调试接口获取最新游戏数据。

## 安装指南

### 源码编译（开发模式）

如需从源码开发或直接运行 SteamCmdPal 扩展：

... 具体步骤

## 使用说明

安装并运行扩展后，打开 Powertoys 命令面板（默认快捷键：`Win` + `Alt` + `Space`）。

### 搜索游戏

在命令面板输入 "SteamPal" 并按 `Enter` 进入扩展页面，随后输入游戏名称，结果列表将实时筛选。

### 启动游戏

从搜索结果选中已安装游戏，按 `Enter` 键（或双击）即可通过 Steam 启动游戏。

### 查看游戏属性

在选中游戏时，可通过以下方式访问属性设置：
按下 `Ctrl + Enter` 组合键，或点击项目关联的附加命令打开 Steam 游戏属性窗口。

### 搜索 Steam 商店

若本地游戏库未找到匹配项，结果底部将出现"在 Steam 中搜索"命令。选择该命令将在默认浏览器打开 Steam 商店并自动搜索。

## 设置选项

通过命令面板的设置界面可配置以下选项：

-   **最大结果显示数**：限制搜索结果数量（建议值：低于 50 以保证性能）。
-   **（可选）调试端口**：手动指定 Steam CEF 调试端口，留空则自动分配。

## 贡献指南

欢迎贡献代码！如发现漏洞或有功能建议，请在 GitHub 仓库提交 issue。欢迎 PR 。

## 开源许可

本项目采用 MIT 许可证授权。