require 'webrick'

server = WEBrick::HTTPServer.new(Port: 1337)

server.mount '/', WEBrick::HTTPServlet::FileHandler, './web'

trap 'INT' do server.shutdown end

server.start