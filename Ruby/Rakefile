require 'rake'
require 'rake/testtask'
require 'rdoc/task'
require 'rubygems/package_task'
require 'rake/contrib/rubyforgepublisher'

task :default => [ :test ]

# Build gem
task :build do
  system "gem build bluepay.gemspec"
end

# Runs tests
Rake::TestTask.new do |t|
  t.libs << 'test'
  t.test_files = FileList['test/*/*.rb']
  t.verbose = false
end

# Genereate the RDoc documentation
desc "Create documentation"
Rake::RDocTask.new("doc") { |rdoc|
  rdoc.title = "Ruby Merchant Bluepay"
  rdoc.rdoc_dir = 'doc'
  rdoc.rdoc_files.include('README')
  rdoc.rdoc_files.include('lib/*.rb')
}

desc "Report code statistics (KLOCs, etc) from the application"
task :stats do
  require 'code_statistics'
  CodeStatistics.new(
    ["Library", "lib"],
    ["Units", "test"]
  ).to_s
end
