# Cursor configuration

## MCP (Model Context Protocol) – Web / Browser

This project enables **web and browser** tools for the Cursor agent via MCP.

### What’s configured

- **`.cursor/mcp.json`** – Defines the **Puppeteer** MCP server so the agent can:
  - Navigate to URLs
  - Take screenshots
  - Click elements, fill forms, run JavaScript in a real browser

### Requirements

- **Node.js** (and `npx`) on your PATH. The server runs via `npx -y @modelcontextprotocol/server-puppeteer` and will download the package on first use.

### How to enable in Cursor

1. **Use project config**  
   Cursor automatically loads `.cursor/mcp.json` when you open this repo. No extra step if the file is there.

2. **Reload if needed**  
   If you added or changed `mcp.json` after opening the project:  
   **Cursor Settings → Tools & MCP** (or **Features → MCP**), then confirm the **puppeteer** server appears and is enabled. Restart Cursor if it doesn’t show up.

3. **Use Agent mode**  
   In Composer, choose **Agent** (not Ask). The agent will see the MCP tools under “Available Tools” and can use them when relevant (e.g. “open this URL and tell me what’s on the page”).

### Optional: global MCP config

To have the same browser server in every project, add it to your **global** MCP config:

- **macOS**: `~/.cursor/mcp.json`
- **Windows**: `%APPDATA%\Cursor\mcp.json`
- **Linux**: `~/.config/cursor/mcp.json`

Use the same `mcpServers.puppeteer` entry as in `.cursor/mcp.json`.

### Cursor’s built-in Browser agent

Cursor also has a built-in **Browser** agent (see [Agent → Browser](https://docs.cursor.com/en/agent/browser)). That’s separate from MCP: it’s a dedicated agent for browser tasks. The Puppeteer MCP gives the **main Composer agent** browser tools inside normal chat/agent sessions.

### More info

- [Cursor: Model Context Protocol (MCP)](https://cursor.com/docs/context/mcp)
- [Cursor: MCP directory](https://cursor.com/docs/context/mcp/directory) – browse and install other servers
